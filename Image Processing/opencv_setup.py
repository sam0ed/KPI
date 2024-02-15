import cv2 as cv
import numpy as np
import imutils
print(cv.__version__)
print(imutils.__version__)

img = cv.imread('assets\cover5.jpg')
img_gray = cv.imread('assets\cover5.jpg', 0)

height, width, _ = img.shape
print(f'The resolution of the image is {width}x{height}')

cv.imshow('this is image', img)
cv.waitKey(0) 
cv.destroyAllWindows()

cv.imwrite('assets\saved.jpg', img)
img = cv.imread('assets\saved.jpg')
cv.imshow('this is image', img)
cv.waitKey(0) 
cv.destroyAllWindows()

(blue, green, red) = img[100, 50]
print(f'{red=}, {green=} {blue=}')

roi = img[ 450:550, 800:1000]
cv.imshow('region', roi)
cv.waitKey()
cv.destroyAllWindows()

resized = cv.resize(img, (800, 200))
cv.imshow('resized', resized)
cv.waitKey()
cv.destroyAllWindows()

h, w = img.shape[0:2]
h_new = 300
ratio = w/h
w_new = int(h_new * ratio)
resized = cv.resize(img, (w_new, h_new))
cv.imshow('resized second way', resized)
cv.waitKey()
cv.destroyAllWindows()

resized = imutils.resize(img, width=300)
cv.imshow('resized second way', resized)
cv.waitKey()
cv.destroyAllWindows()

h, w = resized.shape[0: 2]
center = (w//2, h//2)
M = cv.getRotationMatrix2D(center, -45, 1.0)
rotated = cv.warpAffine(resized, M, (w,h))
cv.imshow('rotated', rotated)
cv.waitKey()
cv.destroyAllWindows()

rotated = imutils.rotate(resized, -45)
cv.imshow('rotated', rotated)
cv.waitKey()
cv.destroyAllWindows()

blurred = cv.GaussianBlur(resized, (11,11), 0)
cv.imshow('blurred', blurred)
cv.waitKey()
cv.destroyAllWindows()

suming = np.hstack((resized, blurred))
cv.imshow('stacked', suming)
cv.waitKey()
cv.destroyAllWindows()

cv.rectangle(resized, (80, 30), (220, 140), (0, 0, 255), 2)
cv.imshow('with rectangle', resized)
cv.waitKey()
cv.destroyAllWindows()

img = np.zeros((220, 200, 3), np.uint8)

#draw a diagonal line
cv.line(img, (0,0), (200, 200), (255, 0, 0), 5)
cv.imshow('the line', img)
cv.waitKey()
cv.destroyAllWindows()

points = np.array([[0, 0], [100, 50], [50, 100], [0, 0]])
cv.polylines(img, np.int32([points]), 1, (255, 255, 255))
cv.imshow('point line', img)
cv.waitKey()
cv.destroyAllWindows()

cv.circle(img, (100, 100), 50, (0, 0, 255), 2)
cv.imshow('circle', img)
cv.waitKey()
cv.destroyAllWindows()

img = np.zeros((220, 700, 3), np.uint8)
font = cv.FONT_HERSHEY_SCRIPT_COMPLEX
cv.putText(img, 'Hello openCV', (0, 100), font, 4, (255,255,255), 4, cv.LINE_4)
cv.imshow('text', img)
cv.waitKey()
cv.destroyAllWindows()