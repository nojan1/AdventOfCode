use aoc;

fn main() {
    println!("Answer to A: {}", part_a("day11/input.txt"));
    println!("Answer to B: {}", part_b("day11/input.txt"));
}

fn on_flash(input: &mut Vec::<Vec::<i64>>, did_flash: &mut Vec::<(usize, usize)>, x: usize, y: usize) {
    if did_flash.contains(&(x,y)) {
        return;
    }
    
    did_flash.push((x,y));

    let x = x as i32;
    let y = y as i32;

    let mut new_flashes = Vec::<(usize, usize)>::new();

    for (xdiff, ydiff) in [(0,1), (1,1), (1,0), (1,-1), (0,-1),(-1,-1),(-1,0),(-1,1)] {
        let new_x = x + xdiff;
        let new_y = y + ydiff;

        if new_x >= 0 && new_y >= 0 && new_y < input.len() as i32 && 
            new_x < input[0].len() as i32 && !did_flash.contains(&(new_x as usize, new_y as usize)) {

            input[new_y as usize][new_x as usize] += 1;

            if input[new_y as usize][new_x as usize] > 9 {
                new_flashes.push((new_x as usize, new_y as usize));
            }
        }
    }

    for (flash_x, flash_y) in new_flashes {
        on_flash(input, did_flash, flash_x, flash_y);
    }

    input[y as usize][x as usize] = 0;
}

fn do_step(input: &mut Vec::<Vec::<i64>>) -> i64 {
    let grid_height = input.len();
    let grid_width = input[0].len();

    let mut did_flash = Vec::<(usize, usize)>::new();

    for y in 0..grid_height {
        for x in 0..grid_width {
            input[y][x] += 1;
        }
    }

    for y in 0..grid_height {
        for x in 0..grid_width {
            if input[y][x] > 9 {
                on_flash(input, &mut did_flash, x, y);
            }
        }
    }

    did_flash.len() as i64
}

fn part_a(filename: &str) -> i64{
    let mut input = aoc::read_grid_into::<i64>(filename);
    let mut num_flashes: i64 = 0;

    for _ in 0..100 {
        num_flashes += do_step(&mut input);
    }

    num_flashes
}

fn part_b(filename: &str) -> i64 {
    let mut input = aoc::read_grid_into::<i64>(filename);
    let mut num_steps: i64 = 0;

    loop {
        num_steps += 1;

        if do_step(&mut input) == 100 {
            break;
        }
    }

    num_steps
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
        assert_eq!(part_b("input_example.txt"), 195);
    }
}
