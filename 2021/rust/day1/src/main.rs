use aoc;
use std::collections::VecDeque;

fn main() {
    println!("Answer to A: {}", part_a("day1/input.txt"));
    println!("Answer to B: {}", part_b("day1/input.txt"));
}

fn part_a(filename: &str) -> u32{
    let ints = aoc::read_ints(filename);

    let mut last: u32 = 0;
    let mut count_greater: u32 = 0;

    for current in ints {
        if last != 0 && current > last {
            count_greater += 1;
        }

        last = current;
    }

    count_greater
}

fn part_b(filename: &str) -> u32 {
    let ints = aoc::read_ints(filename);
    let mut window = VecDeque::from([ints[0], ints[1], ints[2]]);

    let mut last: u32 = 0;
    let mut count_greater: u32 = 0;

    for current in ints {
        let window_sum = window.iter().sum();

        if window_sum > last {
            count_greater += 1;
        }

        window.pop_front();
        window.push_back(current);

        last = window_sum;
    }

    count_greater
}

#[cfg(test)]
mod tests {
    use super::*;

    #[test]
    fn part_a_example_1() {
        assert_eq!(part_a("input_example.txt"), 7);
    }

    #[test]
    fn part_b_example_1() {
        assert_eq!(part_b("input_example.txt"), 5);
    }
}
