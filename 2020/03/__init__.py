from functools import reduce

def parse_grid(data):
    return [[c == "#" for c in row] for row in data.split("\n")]

def is_tree(grid, pos):
    x,y = pos
    return grid[y][x % len(grid[0])]

def check_for_slope(grid, xDiff, yDiff):
    pos = (0,0)
    num_trees = 0
    while pos[1] < len(grid) - 1:
        pos = (pos[0] + xDiff, pos[1] + yDiff)
        if is_tree(grid, pos):
            num_trees+=1

    return num_trees

def part1(data):
    grid = parse_grid(data)
    return check_for_slope(grid, 3, 1)

def part2(data):
    grid = parse_grid(data)
    values = [
        check_for_slope(grid, 1, 1),
        check_for_slope(grid, 3, 1),
        check_for_slope(grid, 5, 1),
        check_for_slope(grid, 7, 1),
        check_for_slope(grid, 1, 2)
    ]

    return reduce(lambda acc,cur: acc * cur, values)

tests = [
    ("03_test1", 7, part1),
    ("03_test1", 336, part2)
]