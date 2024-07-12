# %%
import numpy as np
from graphics import *

# %%
# Rotation matrix around the X-axis
def rotation_matrix_x(angle):
    rad = np.radians(angle)
    return np.array([
        [1, 0, 0, 0],
        [0, np.cos(rad), -np.sin(rad), 0],
        [0, np.sin(rad), np.cos(rad), 0],
        [0, 0, 0, 1]
    ])

# %%
# Rotation matrix around the Y-axis
def rotation_matrix_y(angle):
    rad = np.radians(angle)
    return np.array([
        [np.cos(rad), -np.sin(rad), 0, 0],
        [np.sin(rad), np.cos(rad), 0, 0],
        [0, 0, 1, 0],
        [0, 0, 0, 1]
    ]
    )

# %%
# Rotation matrix around the Z-axis
def rotation_matrix_z(angle):
    rad = np.radians(angle)
    return np.array([
        [np.cos(rad), 0, -np.sin(rad), 0],
        [0, 1, 0, 0],
        [np.sin(rad), 0, np.cos(rad), 0],
        [0, 0, 0, 1]
    ]
    )

# %%
# Axonometric projection matrix
def get_projection_matrix():
    return np.array([
        [1, 0, 0, 0],
        [0, 1, 0, 0],
        [0, 0, 0, 0],
        [0, 0, 0, 1]
    ])


# %%

def project_vertices(vertices, projection_matrix):
    projected = np.dot(vertices, projection_matrix.T)
    return projected[:, :2]  # Return only x and y for 2D drawing


# %%

def draw_pyramid(win, vertices_2d):
    # Draw base
    base = Polygon([Point(*vertex) for vertex in vertices_2d[:4]])
    base.draw(win)

    # Draw sides
    for i in range(4):
        side = Line(Point(vertices_2d[i][0], vertices_2d[i][1]), Point(vertices_2d[4][0], vertices_2d[4][1]))
        side.draw(win)
        


# %%
# Define the pyramid vertices in homogenous coordinates (x, y, z, 1)
pyramid_vertices = np.array([
    [0, 0, 0, 1],  # Base vertices
    [100, 0, 0, 1],
    [100, 100, 0, 1],
    [0, 100, 0, 1],
    [50, 50, 100, 1]  # Top vertex
])

# Create graphics window
win = GraphWin("Pyramid Axonometric Projection", 400, 400)
win.setBackground('white')

rotated_vertices = pyramid_vertices @ rotation_matrix_x(-80)
rotated_vertices = rotated_vertices @ rotation_matrix_z(20)

projection_matrix = get_projection_matrix()

vertices_2d = project_vertices(rotated_vertices, projection_matrix)

# Adjust position for visibility
vertices_2d += np.array([200, 200])  # Center the pyramid in the window

draw_pyramid(win, vertices_2d)

win.getMouse()
win.close()


# %%
print(vertices_2d)

# %%
#------------------------- алгоритм Брезенхема для растрофікації ліній ----------------------------------
def draw_line_pixel (win, x1, y1, x2, y2):
    result = []
    
    dx = x2 - x1;  dy = y2 - y1
    sign_x = 1 if dx > 0 else -1 if dx < 0 else 0
    sign_y = 1 if dy > 0 else -1 if dy < 0 else 0
    if dx < 0: dx = -dx
    if dy < 0: dy = -dy
    if dx > dy:
        pdx, pdy = sign_x, 0
        es, el = dy, dx
    else:
        pdx, pdy = 0, sign_y
        es, el = dx, dy
    x, y = x1, y1
    error, t = el / 2, 0

    win.plot(x, y, 'blue')
    result.append((x,y))

    while t < el:
        error -= es
        if error < 0:
            error += el
            x += sign_x
            y += sign_y
        else:
            x += pdx
            y += pdy
        t += 1
        win.plot(x, y, 'blue')
        result.append((x,y))

    return result

# %%
# win = GraphWin("Rasterized Pyramid", 400, 400)
# win.setBackground('white')

# draw_line_pixel(win, vertices_2d[0,0], vertices_2d[0,1], vertices_2d[1,0], vertices_2d[0,1])
# draw_line_pixel(win, vertices_2d[0,0], vertices_2d[0,1], vertices_2d[4,0], vertices_2d[4,1])

# win.getMouse()
# win.close()

# %%
def draw_pyramid_edges(win, vertices_2d):
    # Draw the base of the pyramid
    for i in range(4):
        draw_line_pixel(win, vertices_2d[i][0], vertices_2d[i][1], vertices_2d[(i+1) % 4][0], vertices_2d[(i+1) % 4][1])
    # Draw the sides connecting the base to the apex
    apex = vertices_2d[4]
    for i in range(4):
        draw_line_pixel(win, vertices_2d[i][0], vertices_2d[i][1], apex[0], apex[1])

# %%
win = GraphWin("Rasterized Pyramid", 400, 400)
win.setBackground('white')

# Draw all edges of the pyramid
draw_pyramid_edges(win, vertices_2d)

win.getMouse()  
win.close()    



