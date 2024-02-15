import tkinter as tk
import numpy as np
import random

# Define the pyramid vertices in homogenous coordinates (x, y, z, 1)
pyramid_vertices = np.array([
    # [0, 0, 0, 1],  # Base vertices
    # [100, 0, 0, 1],
    # [100, 100, 0, 1],
    # [0, 100, 0, 1],
    # [50, 50, 100, 1]  # Top vertex
    
    [0, 0, 0, 1],  # Base vertices
    [0, 0, 100, 1],# Top vertex
    [0, 100, 100, 1],
    [0, 100, 0, 1],
    [100, 50, 50, 1],
])

# Rotation matrix around the Y-axis
def rotation_matrix_y(angle):
    rad = np.radians(angle)
    return np.array([
        [np.cos(rad), 0, np.sin(rad), 0],
        [0, 1, 0, 0],
        [-np.sin(rad), 0, np.cos(rad), 0],
        [0, 0, 0, 1]
    ])

# def rotation_matrix_y(angle):
#     rad = np.radians(angle)
#     return np.array([
#         [1, 0, 0, 0],
#         [0, np.cos(rad), -np.sin(rad), 0],
#         [0, np.sin(rad), np.cos(rad), 0],
#         [0, 0, 0, 1]
#     ])


# Axonometric projection matrix
def axonometric_projection():
    # Simple isometric projection
    # angle = np.radians(35.264)  # 35.264 degrees for isometric
    angle = np.radians(0)  # 35.264 degrees for isometric
    return np.array([
        [1, 0, -np.cos(angle), 0],
        [0, 1, -np.sin(angle), 0],
        [0, 0, 0, 0],
        [0, 0, 0, 1]
    ])

# Convert 3D vertices to 2D vertices
def project_vertices(vertices, projection_matrix):
    projected = vertices @ projection_matrix.T
    return projected[:, :2]  # Return only x and y for 2D drawing

# Draw the pyramid on a Tkinter canvas
def draw_pyramid(canvas, vertices_2d, color):
    # Clear previous drawing
    canvas.delete("all")
    # Draw base
    base = vertices_2d[:4].flatten().tolist()
    canvas.create_polygon(base, outline=color, fill='')
    # Draw sides
    for i in range(4):
        side = [vertices_2d[i][0], vertices_2d[i][1], vertices_2d[4][0], vertices_2d[4][1]]
        canvas.create_line(side, fill=color)

# Main animation function
def animate_pyramid(canvas, angle=0):
    global pyramid_vertices
    # Rotate the pyramid
    rotated_vertices = pyramid_vertices @ rotation_matrix_y(angle)
    # Project the rotated vertices to 2D
    projection_matrix = axonometric_projection()
    vertices_2d = project_vertices(rotated_vertices, projection_matrix)
    # Randomize position and color
    offset_x, offset_y = 120, 120 #random.randint(50, 350), random.randint(50, 350)
    color = random.choice(['red', 'green', 'blue', 'yellow', 'purple'])
    # Draw the pyramid
    draw_pyramid(canvas, vertices_2d + np.array([offset_x, offset_y]), color)
    # Schedule next animation frame
    canvas.after(1000, lambda: animate_pyramid(canvas, angle + 5))

# Setup Tkinter window and canvas
root = tk.Tk()
root.title("3D Pyramid Animation with Tkinter")
canvas = tk.Canvas(root, width=400, height=400, bg='white')
canvas.pack()

# Start the animation
animate_pyramid(canvas)
draw_pyramid(canvas, pyramid_vertices, 'red')

root.mainloop()



