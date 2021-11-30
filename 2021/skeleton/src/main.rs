fn main() {
    println!("Answer to A: {}", part_a("day1/input.txt"));
    println!("Answer to B: {}", part_b("day1/input.txt"));
}

fn part_a(filename: &str) -> i64{
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
        assert_eq!(part_a("input_example.txt"), 0);
    }

    #[test]
    fn part_b_example_1() {
        assert_eq!(part_b("input_example.txt"), 0);
    }
}
