use aoc;
use std::collections::{HashMap, VecDeque};

fn main() {
    println!("Answer to A: {}", part_a("day13/input.txt"));
    part_b("day13/input.txt");
}

enum Fold {
    X(i32),
    Y(i32)
}

fn parse_input(filename: &str) -> (HashMap::<(i32,i32), bool>, VecDeque::<Fold>) {
    let mut points = HashMap::<(i32,i32), bool>::new();
    let mut folds = VecDeque::<Fold>::new();

    let mut lines = aoc::read_lines(filename).into_iter();

    loop {
        let line = lines.next().unwrap();
        if line == "" {
            break;
        }

        let parts = line.split(",").collect::<Vec::<&str>>();
        let x: i32= parts[0].parse().unwrap();
        let y: i32 = parts[1].parse().unwrap();

        points.insert((x,y), true);
    }

    while let Some(line) = lines.next() {
        let direction: &str = &line[11..12];
        let amount: i32 = line[13..].parse().unwrap();

        folds.push_back(match direction {
            "x" => Fold::X(amount),
            "y" => Fold::Y(amount),
            _ => panic!("Bhuuu")
        });
    }

    (points, folds)
}

fn do_fold(fold: Fold, points:&mut  HashMap::<(i32,i32), bool>) {
    let mut to_remove = Vec::<(i32,i32)>::new();
    let mut to_add = Vec::<(i32,i32)>::new();

    for ((x,y), _) in points.iter() {
        match fold {
            Fold::X(limit) => { if *x < limit { continue; } },
            Fold::Y(limit) => { if *y < limit { continue; } }
        }

        to_remove.push((*x, *y));
        
        match fold {
            Fold::X(breakpoint) => {
                to_add.push((
                    breakpoint - (*x - breakpoint),
                    *y 
                ));
            },
            Fold::Y(breakpoint) => {
                to_add.push((
                    *x,
                    breakpoint - (*y - breakpoint) 
                ));
            },
        }
    }

    for r in &to_remove {
        points.remove(r);
    }

    for a in to_add {
        points.entry(a).or_insert(true);
    }
}

fn part_a(filename: &str) -> i64{
    let (mut points, mut folds) = parse_input(filename);

    do_fold(folds.pop_front().unwrap(), &mut points);

    points.len() as i64
}

fn part_b(filename: &str) {
    let (mut points, mut folds) = parse_input(filename);

    loop {
        if let Some(fold) = folds.pop_front()  {
            do_fold(fold, &mut points);
        }else{
            break;
        }
    }

    for y in 0..10 {
        for x in 0..40 {
            if let Some(_) = points.get(&(x,y)) {
                print!("#");
            }else{
                print!(" ");
            }
        }

        println!("");
    }

}

#[cfg(test)]
mod tests {
    use super::*;

    #[test]
    fn part_a_example_1() {
        assert_eq!(part_a("input_example.txt"), 17);
    }
}
