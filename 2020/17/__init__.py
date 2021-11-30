def get_initial_active(data):
    cubes = []
    extentX = 0
    extentY = 0

    for y,line in enumerate(data.split("\n")):
        for x,c in enumerate(line):
            if c == "#":
                cubes.append((x, y, 0, 0))

            extentX = max(extentX, x)
        extentY = max(extentY, y)

    return cubes, ((-1, extentX+1), (-1, extentY+1), (-1, 1), (-1,1))

def run_cycles(active_cubes, extents, useFourDimensions):
    def get_active_neighbours_count(cubes, center_x, center_y, center_z, center_w):
        active_count = 0
        for x in range(center_x-1, center_x+2):
            for y in range(center_y-1, center_y+2):
                for z in range(center_z-1, center_z+2):
                    if useFourDimensions:
                        for w in range(center_w-1, center_w+2):
                            if x == center_x and y == center_y and z == center_z and w == center_w:
                                continue

                            if (x,y,z,w) in cubes:
                                active_count += 1
                    else:
                        if x == center_x and y == center_y and z == center_z:
                            continue

                        if (x,y,z,0) in cubes:
                            active_count += 1

        return active_count

    extentX, extentY, extentZ, extentW = extents
    cubes = list(active_cubes)

    for i in range(6):
        new_cubes = list(cubes)
        for x in range(extentX[0], extentX[1] + 1):
            for y in range(extentY[0], extentY[1] + 1):
                for z in range(extentZ[0], extentZ[1] + 1):
                    if useFourDimensions:
                        for w in range(extentW[0], extentW[1]+1):
                            neighbour_count = get_active_neighbours_count(cubes, x, y ,z, w)
                            is_active = (x,y,z,w) in cubes
                            if is_active and (neighbour_count < 2 or neighbour_count > 3):
                                    new_cubes.remove((x,y,z,w))
                            elif not is_active and neighbour_count == 3:
                                    new_cubes.append((x,y,z,w))
                    else:
                        neighbour_count = get_active_neighbours_count(cubes, x, y ,z, 0)
                        is_active = (x,y,z,0) in cubes
                        if is_active and (neighbour_count < 2 or neighbour_count > 3):
                                new_cubes.remove((x,y,z,0))
                        elif not is_active and neighbour_count == 3:
                                new_cubes.append((x,y,z,0))
        
        extentX = (extentX[0]-1, extentX[1]+1)
        extentY = (extentY[0]-1, extentY[1]+1)
        extentZ = (extentZ[0]-1, extentZ[1]+1)

        if useFourDimensions:
            extentW = (extentW[0]-1, extentW[1]+1)

        cubes = new_cubes
        
    return len(cubes)

def part1(data):
    active_cubes, extents = get_initial_active(data)
    return run_cycles(active_cubes, extents, False)

def part2(data):
    active_cubes, extents = get_initial_active(data)
    return run_cycles(active_cubes, extents, True)

tests = [
    ("17_test1", 112, part1),
    ("17_test1", 848, part2),
    # ("<input file name minus txt>", <expected result>, part1),
]
