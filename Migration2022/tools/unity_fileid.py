"""Unity's local file ID for a MonoScript stored in a managed DLL: the first four bytes
(little-endian int32) of MD4("s\\0\\0\\0" + namespace + class name)."""
import struct


def _md4(data):
    def f(x, y, z): return (x & y) | (~x & z)
    def g(x, y, z): return (x & y) | (x & z) | (y & z)
    def h(x, y, z): return x ^ y ^ z
    def rol(v, s): v &= 0xFFFFFFFF; return ((v << s) | (v >> (32 - s))) & 0xFFFFFFFF

    msg = bytearray(data)
    bit_len = (8 * len(data)) & 0xFFFFFFFFFFFFFFFF
    msg.append(0x80)
    while len(msg) % 64 != 56:
        msg.append(0)
    msg += struct.pack("<Q", bit_len)
    a, b, c, d = 0x67452301, 0xEFCDAB89, 0x98BADCFE, 0x10325476
    for off in range(0, len(msg), 64):
        x = struct.unpack("<16I", msg[off:off + 64])
        aa, bb, cc, dd = a, b, c, d
        for i in (0, 4, 8, 12):
            a = rol(a + f(b, c, d) + x[i], 3)
            d = rol(d + f(a, b, c) + x[i + 1], 7)
            c = rol(c + f(d, a, b) + x[i + 2], 11)
            b = rol(b + f(c, d, a) + x[i + 3], 19)
        for i in (0, 1, 2, 3):
            a = rol(a + g(b, c, d) + x[i] + 0x5A827999, 3)
            d = rol(d + g(a, b, c) + x[i + 4] + 0x5A827999, 5)
            c = rol(c + g(d, a, b) + x[i + 8] + 0x5A827999, 9)
            b = rol(b + g(c, d, a) + x[i + 12] + 0x5A827999, 13)
        for i in (0, 2, 1, 3):
            a = rol(a + h(b, c, d) + x[i] + 0x6ED9EBA1, 3)
            d = rol(d + h(a, b, c) + x[i + 8] + 0x6ED9EBA1, 9)
            c = rol(c + h(d, a, b) + x[i + 4] + 0x6ED9EBA1, 11)
            b = rol(b + h(c, d, a) + x[i + 12] + 0x6ED9EBA1, 15)
        a, b, c, d = (a + aa) & 0xFFFFFFFF, (b + bb) & 0xFFFFFFFF, (c + cc) & 0xFFFFFFFF, (d + dd) & 0xFFFFFFFF
    return struct.pack("<4I", a, b, c, d)


def script_file_id(namespace, name):
    digest = _md4(b"s\0\0\0" + (namespace + name).encode("utf-8"))
    return struct.unpack("<i", digest[:4])[0]


if __name__ == "__main__":
    import hashlib
    try:
        assert _md4(b"abc") == hashlib.new("md4", b"abc").digest()
    except ValueError:
        pass
    assert _md4(b"abc").hex() == "a448017aaf21d8525fc10ae87aa6729d"
    print("md4 ok")
