def pmt(i, l, p, m, n):
    return p * ( i / (1 - (1 + i) ** -l))

def ipmt(i, num, l, p, a, b):
    m = pmt(i, l, p, a, b)
    n = 1
    while True:
        h = p * i
        if (n >= num):
            return h
        c = m - h
        p = p - c
        if (p < 0):
            return 0
        n = n + 1

def ppmt(i, num, l, p, a, b):
    m = pmt(i, l, p, a, b)
    n = 1
    while True:
        h = p * i
        c = m - h
        if (n >= num):
            return c
        p = p - c
        if (p < 0):
            return 0
        n = n + 1

print pmt(0.05 / 12, 360, 82478, 0, 0)
print ipmt(0.05 / 12, 600, 360, 82478, 0, 0)
print ppmt(0.05 / 12, 600, 360, 82478, 0, 0)
