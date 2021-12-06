use std::fs;
use std::str::FromStr;
use std::fmt::Debug;

pub fn read_grid(filename: &str) -> Vec<Vec<char>>{
    fs::read_to_string(filename).unwrap()
        .lines()
        .map(|x| x.chars().collect())
        .collect::<Vec<Vec<char>>>()
} 

pub fn read_ints(filename: &str) -> Vec<u32> {
    fs::read_to_string(filename).unwrap()
        .lines()
        .map(|x| x.parse().expect("Not an integer"))
        .collect()
}

pub fn read_int_list(filename: &str) -> Vec<u32> {
    fs::read_to_string(filename).unwrap()
        .split(",")
        .map(|x| x.parse().expect("Not an integer"))
        .collect()
}

pub fn read_lines(filename: &str) -> Vec<String> {
    fs::read_to_string(filename).unwrap()
        .lines()
        .map(String::from)
        .collect()
}

pub fn read_into<T: FromStr>(filename: &str) -> Vec<T> where <T as FromStr>::Err: Debug {
    fs::read_to_string(filename).unwrap()
        .lines()
        .map(|x| str::parse(x).unwrap())
        .collect()
}

pub fn grid_iterate(width: u32, height: u32) -> impl std::iter::Iterator<Item = (i32,i32)> 
{
    let mut x = 0;
    let mut y = 0;

    std::iter::from_fn(move || {
        x += 1;
        if x > 0 && x % width == 0 {
            x = 0;
            y += 1;
        }

        if y > 0 && y % height == 0 {
            return None;
        }

        return Some((x as i32,y as i32));
    })
}

// #[cfg(test)]
// mod tests {
//     #[test]
//     fn it_works() {
//         let result = 2 + 2;
//         assert_eq!(result, 4);
//     }
// }
