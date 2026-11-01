"""Compare recovered scene hierarchy with the original build's decoded fields."""
from pathlib import Path
import gzip
import json
import math
import re
import sys
sys.path.insert(0,str(Path(__file__).resolve().parents[3]/'skill-packages/validation-deps'))
import yaml

ROOT=Path(__file__).resolve().parents[2]
GAME=ROOT.parent


def equivalent(a,b):
    if isinstance(a,dict) and isinstance(b,dict):
        if 'm_PathID' in a and 'fileID' in b:
            return int(a['m_FileID'])==0 and int(a['m_PathID'])==int(b['fileID'])
        return a.keys()==b.keys() and all(equivalent(a[k],b[k]) for k in a)
    if isinstance(a,list) and isinstance(b,list):
        return len(a)==len(b) and all(equivalent(x,y) for x,y in zip(a,b))
    if isinstance(a,(float,int)) and isinstance(b,(float,int)):
        return math.isclose(a,b,rel_tol=1e-6,abs_tol=1e-6)
    return a==b


def main():
    inventory=json.loads((GAME/'asset-extraction/evidence/objects/inventory.json').read_text())
    results=[]
    for source_name,scene in [('level0','Bootstrap'),('level1','SampleScene')]:
        source=next(s for s in inventory['sources'] if s['path'].endswith('/'+source_name))
        source_path=GAME/'asset-extraction/evidence/objects'/(source['token']+'.trees.jsonl.gz')
        originals={}
        with gzip.open(source_path,'rt',encoding='utf-8') as stream:
            for line in stream:
                row=json.loads(line)
                originals[row['path_id']]=row
        text=(ROOT/'EndlessRails-2022/Assets/Scenes'/(scene+'.unity')).read_text(encoding='utf-8-sig')
        chunks=re.split(r'^--- !u!(-?\d+) &(-?\d+)\r?\n',text,flags=re.M)
        current={}
        for index in range(1,len(chunks),3):
            value=yaml.safe_load(chunks[index+2])
            current[chunks[index+1]]={'class_id':int(chunks[index]),'fields':next(iter(value.values()))}
        selected={'GameObject':['m_Name','m_Layer','m_IsActive','m_Component'], 'Transform':['m_LocalPosition','m_LocalRotation','m_LocalScale','m_Father','m_Children'], 'RectTransform':['m_LocalPosition','m_LocalRotation','m_LocalScale','m_Father','m_Children','m_AnchorMin','m_AnchorMax','m_AnchoredPosition','m_SizeDelta','m_Pivot']}
        result={'scene':scene,'objects_compared':0,'fields_compared':0,'mismatches':[],'source_decoded_gameobjects':sum(o['type']=='GameObject' for o in originals.values())}
        for path_id,original in originals.items():
            if original['type'] not in selected:
                continue
            result['objects_compared']+=1
            if path_id not in current:
                result['mismatches'].append({'path_id':path_id,'error':'missing object'})
                continue
            actual=current[path_id]['fields']
            for field in selected[original['type']]:
                if field not in original['fields']:
                    continue
                expected=original['fields'][field]
                value=actual.get(field)
                if field=='m_Name' and value is not None:
                    value=str(value)
                if field=='m_Component':
                    expected=[item['component'] if isinstance(item,dict) and 'component' in item else item for item in expected]
                    value=[item['component'] for item in value]
                result['fields_compared']+=1
                if not equivalent(expected,value):
                    result['mismatches'].append({'path_id':path_id,'type':original['type'],'field':field,'source':expected,'project':value})
        results.append(result)
    (ROOT/'migration-2022/evidence/source-scene-comparison.json').write_text(json.dumps(results,indent=2),encoding='utf-8')
    print(json.dumps([{k:v for k,v in row.items() if k!='mismatches'} | {'mismatches':len(row['mismatches'])} for row in results]))


if __name__=='__main__':
    main()
