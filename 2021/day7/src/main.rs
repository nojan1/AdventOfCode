use aoc;

fn main() {
    println!("Answer to A: {}", part_a("day7/input.txt"));
    println!("Answer to B: {}", part_b("day7/input.txt"));
}

fn find_cheapest_move(filename: &str, use_simple_cost: bool) -> i32 {
    let data = aoc::read_int_list(filename);
    
    let mut costs = Vec::<i32>::new();
    let max_position = data.iter().fold(std::u32::MIN, |a,b| a.max(*b));

    for position in 0..max_position {
        let mut current_cost = 0;
        for submarine in &data {
            let distance = (*submarine as i32 - position as i32).abs();
            current_cost += if use_simple_cost {
                distance
            } else {
                (0..distance + 1).fold(0, |a, b| a + b)
            };
        }

        costs.push(current_cost);
    }

    costs.iter().fold(std::i32::MAX, |a,b| a.min(*b))
}

fn part_a(filename: &str) -> i32{
    find_cheapest_move(filename, true)
}

fn part_b(filename: &str) -> i32 {
    find_cheapest_move(filename, false)
}

#[cfg(test)]
mod tests {
    use super::*;

    #[test]
    fn part_a_example_1() {
        assert_eq!(part_a("input_example.txt"), 37);
    }

    #[test]
    fn part_b_example_1() {
        assert_eq!(part_b("input_example.txt"), 168);
    }
}
