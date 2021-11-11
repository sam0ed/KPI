def pow2(a: int):
    if a==1:
        return 1
    return pow2(a-1) +2*(a-1) +1

n = int(input("Enter the value:"))
print(pow2(n) if n > 0 else "Enter other value")

