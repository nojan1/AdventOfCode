use aoc;
use std::collections::VecDeque;

fn main() {
    println!("Answer to A: {}", part_a("day6/input.txt"));
    println!("Answer to B: {}", part_b("day6/input.txt"));
}

fn run_simulation(filename: &str, num_cycles: u32) -> u64 {
    let initial_state = aoc::read_int_list(filename);
    let mut state = VecDeque::<u64>::new();

    for i in 0..9 {
        let fish_age_count = initial_state.iter().filter(|x| **x == i).count();
        state.push_back(fish_age_count as u64);
    }

    for _ in 0..num_cycles {
        let current_age = state.pop_front().unwrap();

        state[6] += current_age;        
        state.push_back(current_age);
    }

    state.iter().sum()
}

fn part_a(filename: &str) -> u64{
  run_simulation(filename, 80)
}

fn part_b(filename: &str) -> u64 {
    run_simulation(filename, 256)
}

#[cfg(test)]
mod tests {
    use super::*;

    #[test]
    fn part_a_example_1() {
        assert_eq!(part_a("input_example.txt"), 5934);
    }

    #[test]
    fn part_b_example_1() {
        assert_eq!(part_b("input_example.txt"), 26984457539);
    }
}
