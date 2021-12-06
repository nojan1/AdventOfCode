use aoc;

fn main() {
    println!("Answer to A: {}", part_a("day3/input.txt"));
    println!("Answer to B: {}", part_b("day3/input.txt"));
}

fn is_1_most_common(data: &Vec<Vec<char>>, index: usize) -> bool {
    let mut num_one = 0;
    let mut num_zero = 0;

    for number in data {
        if number[index] == '1' {
            num_one += 1;
        }else{
            num_zero += 1;
        }
    }

    num_one >= num_zero
}

fn part_a(filename: &str) -> u32{
    let data = aoc::read_grid(filename);

    let digit_length = data[0].len();
    let mut gamma_rate: u32 = 0;
    for x in 0..digit_length {
        if is_1_most_common(&data, x){
            gamma_rate += 1 << (digit_length - x - 1);
        }
    }

    let epislon_rate = (!gamma_rate) & (u32::pow(2, digit_length as u32) - 1);

    gamma_rate * epislon_rate
}

fn sift_numbers(mut data: Vec<Vec<char>>, sift_on_most_common: bool) -> u32 {
    let digit_length = data[0].len();

    loop {
        for x in 0..digit_length {
            let one_most_common = is_1_most_common(&data, x);

            data.retain(|item| 
                (one_most_common && sift_on_most_common && item[x] == '1') ||
                (!one_most_common && sift_on_most_common && item[x] == '0') ||
                (one_most_common && !sift_on_most_common && item[x] == '0') ||
                (!one_most_common && !sift_on_most_common && item[x] == '1')
            );

            if data.len() == 1 {
                let data_string = data[0].clone().into_iter().collect::<String>();
                return u32::from_str_radix(&data_string[..], 2).unwrap();
            }
        }
    }
}

fn part_b(filename: &str) -> u32 {
    let data = aoc::read_grid(filename);
    let oxygen_generator_rating = sift_numbers(data.clone(), true);
    let co2_scrubber_rating = sift_numbers(data.clone(), false);
    
    oxygen_generator_rating * co2_scrubber_rating
}

#[cfg(test)]
mod tests {
    use super::*;

    #[test]
    fn part_a_example_1() {
        assert_eq!(part_a("input_example.txt"), 198);
    }

    #[test]
    fn part_b_example_1() {
        assert_eq!(part_b("input_example.txt"), 230);
    }
}
