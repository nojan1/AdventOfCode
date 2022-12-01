use aoc;

fn main() {
    println!("Answer to A: {}", part_a("day9/input.txt"));
    println!("Answer to B: {}", part_b("day9/input.txt"));
}

fn is_low_spot(x: usize, y: usize, height_map: &Vec::<Vec::<i64>>) -> bool {
    let my_value = height_map[y][x];

    (x == 0 || height_map[y][x - 1] > my_value) &&
    (x == height_map[0].len() - 1 || height_map[y][x + 1] > my_value) &&
    (y == 0 || height_map[y - 1][x] > my_value) &&
    (y == height_map.len() - 1 || height_map[y + 1][x] > my_value)
}

fn find_low_spots(height_map: &Vec::<Vec::<i64>>) -> (i64, Vec::<(usize,usize)>) {
    let grid_height = height_map.len();
    let grid_width = height_map[0].len();

    let mut low_spots = Vec::<(usize, usize)>::new();
    let mut risk_scores = 0;
    let mut grid_iterator = aoc::grid_iterate(grid_width as u32, grid_height as u32);
    while let Some((x,y)) = grid_iterator.next() {
        if is_low_spot(x as usize, y as usize, &height_map) {
            risk_scores += height_map[y as usize][x as usize] + 1;  
            low_spots.push((x as usize,y as usize));
        }
    }

    (risk_scores, low_spots)
}

fn discover_basin_sizes(low_spots: &Vec::<(usize,usize)>, height_map: &Vec::<Vec::<i64>>) -> Vec::<i64> {
    let find_basin_at = |x: usize, y: usize| -> i64 {

        fn find_basin_inner(x: usize, y: usize, found: &mut Vec::<(usize,usize)>, height_map: &Vec::<Vec::<i64>>) {
            let grid_height = height_map.len();
            let grid_width = height_map[0].len();    

            if x > grid_width - 1 || y > grid_height - 1 || height_map[y][x] == 9 || found.iter().any(|c| c.0 == x && c.1 == y) {
                return
            }

            found.push((x,y));

            if x > 0 {
                find_basin_inner(x - 1, y, found, height_map);
            }

            if y > 0 {
                find_basin_inner(x, y - 1, found, height_map);
            }

            find_basin_inner(x + 1, y, found, height_map);
            find_basin_inner(x, y + 1, found, height_map);
        }
        
        let mut found = Vec::<(usize, usize)>::new();
        find_basin_inner(x, y, &mut found, height_map);

        found.len() as i64
    };
    
    low_spots.iter()
        .map(|c| find_basin_at(c.0, c.1))
        .collect()
}

fn part_a(filename: &str) -> i64{
    let height_map = aoc::read_grid_into::<i64>(filename);
    let (risk_scores, _) = find_low_spots(&height_map);
   
    risk_scores
}

fn part_b(filename: &str) -> i64 {
    let height_map = aoc::read_grid_into::<i64>(filename);
    let (_, low_spots) = find_low_spots(&height_map);
    let mut basin_sizes = discover_basin_sizes(&low_spots, &height_map);
    basin_sizes.sort();
    basin_sizes.reverse();
    
    basin_sizes[0] * basin_sizes[1] * basin_sizes[2]
}

#[cfg(test)]
mod tests {
    use super::*;

    #[test]
    fn part_a_example_1() {
        assert_eq!(part_a("input_example.txt"), 15);
    }

    #[test]
    fn part_b_example_1() {
        assert_eq!(part_b("input_example.txt"), 1134);
    }
}
