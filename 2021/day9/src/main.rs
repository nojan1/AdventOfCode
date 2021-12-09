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
    let grid_height = &height_map.len();
    let grid_width = &height_map[0].len();

    let mut low_spots = Vec::<(usize, usize)>::new();
    let mut risk_scores = 0;
    let mut grid_iterator = aoc::grid_iterate(*grid_width as u32, *grid_height as u32);
    while let Some((x,y)) = grid_iterator.next() {
        if is_low_spot(x as usize, y as usize, &height_map) {
            risk_scores += height_map[y as usize][x as usize] + 1;  
            low_spots.push((x as usize,y as usize));
        }
    }

    (risk_scores, low_spots)
}

fn part_a(filename: &str) -> i64{
    let height_map = aoc::read_grid_into::<i64>(filename);
    let (risk_scores, _) = find_low_spots(&height_map);
   
    risk_scores
}

fn part_b(filename: &str) -> i64 {
    0
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
        assert_eq!(part_b("input_example.txt"), 0);
    }
}
