use std::str::FromStr;

fn main() {
    println!("Answer to A: {}", part_a("day5/input.txt"));
    println!("Answer to B: {}", part_b("day5/input.txt"));
}

struct Line {
    from: (i32, i32),
    to: (i32, i32),
    k: i32,
    m: i32,
}

impl Line {
    fn new(p0: (i32, i32), p1: (i32, i32)) -> Line {
        let from = if p0.0 < p1.0 || p0.1 < p1.1 { p0 } else { p1 };
        let to = if p0.0 < p1.0 || p0.1 < p1.1 { p1 } else { p0 };

        if p0.0 == p1.0 || p0.1 == p1.1 {
            Line {
                from,
                to,
                k: 0,
                m: 0,
            }
        } else {
            let k = (from.1 - to.1) / (from.0 - to.0);
            let m = to.1 - (k * to.0);

            Line {
                from,
                to,
                k: k,
                m: m,
            }
        }
    }

    fn crosses(&self, point: (i32, i32)) -> bool {
        if self.k == 0 {
            let does_cross =
                (self.from.0 == point.0 && point.1 >= self.from.1 && point.1 <= self.to.1)
                    || (self.from.1 == point.1 && point.0 >= self.from.0 && point.0 <= self.to.0);

            return does_cross;
        } else {
            if (point.0 < self.from.0 && point.0 < self.to.0) || 
                (point.0 > self.to.0 && point.0 > self.from.0) || 
                (point.1 < self.from.1 && point.1 < self.to.1) || 
                (point.1 > self.from.1 && point.1 > self.to.1) {
                return false;
            }

            return point.1 == (point.0 * self.k) + self.m;
        }
    }
}

impl FromStr for Line {
    type Err = ();

    fn from_str(s: &str) -> Result<Line, ()> {
        fn to_point(point_str: &str) -> (i32, i32) {
            let ints: Vec<i32> = point_str.split(",").map(|x| x.parse().unwrap()).collect();

            (ints[0], ints[1])
        }

        // 0,9 -> 5,9
        let halfs: Vec<&str> = s.split(" -> ").collect();
        Ok(Line::new(to_point(halfs[0]), to_point(halfs[1])))
    }
}

fn count_overlaps(filename: &str, include_horizontal: bool) -> u32 {
    let lines = aoc::read_into::<Line>(filename);

    let mut point_count = 0;
    let mut iter = aoc::grid_iterate(1000,1000);
    while let Some((x,y)) = iter.next() {
        let mut num_lines_crosses = 0;
        for line in &lines {
            if !include_horizontal && line.k != 0 {
                continue;
            }

            if line.crosses((x, y)) {
                num_lines_crosses += 1;
            }
        }

        if num_lines_crosses >= 2 {
            point_count += 1;
        }
    }

    point_count
}

fn part_a(filename: &str) -> u32 {
    count_overlaps(filename, false)
}

fn part_b(filename: &str) -> u32 {
    count_overlaps(filename, true)
}

#[cfg(test)]
mod tests {
    use super::*;

    #[test]
    fn part_a_example_1() {
        assert_eq!(part_a("input_example.txt"), 5);
    }

    #[test]
    fn part_b_example_1() {
        assert_eq!(part_b("input_example.txt"), 12);
    }
}