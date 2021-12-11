use aoc;

fn main() {
    println!("Answer to A: {}", part_a("day11/input.txt"));
    println!("Answer to B: {}", part_b("day11/input.txt"));
}

fn check_neighbours(input: &mut Vec::<Vec::<i64>>, did_flash: &mut Vec::<(usize, usize)>, x: usize, y: usize) {

}

fn part_a(filename: &str) -> i64{
    let mut input = aoc::read_grid_into::<i64>(filename);

    let grid_height = input.len();
    let grid_width = input[0].len();

    for _ in 0..100 {
        //Steps
        let mut did_flash = Vec::<(usize, usize)>::new();

        for y in 0..grid_height {
            for x in 0..grid_width {
                input[y][x] += 1;
            }
        }

        for y in 0..grid_height {
            for x in 0..grid_width {
                if input[y][x] >= 9 {
                    //Flashing!!!!
                    did_flash.push((x,y));

                    check_neighbours(&mut input, &mut did_flash, x, y);

                    input[y][x] = 0;
                }
            }
        }
    }

    0
}

fn part_b(filename: &str) -> i64 {
    0
}

#[cfg(test)]
mod tests {
    use super::*;

    #[test]
    fn part_a_example_1() {
        assert_eq!(part_a("input_example.txt"), 1656);
    }

    #[test]
    fn part_b_example_1() {
        assert_eq!(part_b("input_example.txt"), 0);
    }
}
