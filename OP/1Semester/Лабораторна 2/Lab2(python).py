import math
import os
a=float(input("Enter a parameter:"))
if a > 0:
    x = (-abs(a - 1)) /( 2*a)
    print("The root of equation F(x)=2ax+|a-1|=0 is x= " + str(x))    
else:
	x = math.log(math.sqrt(1 + pow(a, 2)))
	print( "\nThe root of equation F(x)=(e^x/sqrt(1+a^2))-1=0 is x=" , x )
os.system("pause")
