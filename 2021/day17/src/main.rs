use std::cmp::{max, min};

struct TargetArea {
    x_from: i64,
    x_to: i64,

    y_from: i64,
    y_to: i64
}

fn main() {
    let target_area = TargetArea {
        x_from: 25,
        x_to: 67,
        y_from: -260,
        y_to: -200
    };

    let (a,b) = try_trajectories(&target_area);

    println!("Answer to A: {}", a);
    println!("Answer to B: {}", b);
}

fn test_trajectory(mut x_vel: i64, mut y_vel: i64, target_area: &TargetArea) -> (bool, i64) {
    let mut pos_x = 0;
    let mut pos_y = 0;
    let mut high_y = 0;

    loop {
        pos_x += x_vel;
        pos_y += y_vel;

        high_y = max(high_y, pos_y);

        if x_vel < 0 {
            x_vel += 1;
        }else if x_vel > 0 {
            x_vel -= 1;
        }

        y_vel -= 1;

        if pos_x >= target_area.x_from && pos_x <= target_area.x_to && pos_y >= target_area.y_from && pos_y <= target_area.y_to {
            return (true, high_y);
        } 

        if pos_y < min(target_area.y_from, target_area.y_to){
            return (false, 0);
        }
    }
}

fn try_trajectories(target_area: &TargetArea) -> (i64, i64){
    let mut count = 0;
    let mut total_highest = 0;

    let x_limit = max(target_area.x_from.abs(), target_area.x_to.abs());
    let y_limit = max(target_area.y_from.abs(), target_area.y_to.abs());

    for x in (-1 * x_limit)..x_limit + 1 {
        for y in (-1 * y_limit)..y_limit + 1 {
            let (did_hit, highest) = test_trajectory(x, y, target_area);
            if did_hit {
                count += 1;
                total_highest = max(total_highest, highest);
            }
        }
    }

    (total_highest, count)
}

#[cfg(test)]
mod tests {
    use super::*;

    fn get_test_data() -> TargetArea {
        TargetArea {
            x_from: 20,
            x_to: 30,
            y_from: -10,
            y_to: -5
        }
    }

    #[test]
    fn trajectory_calculated_correctly() {
        let target_area = get_test_data();
        assert_eq!(test_trajectory(7, 2, &target_area).0, true);
    }

    #[test]
    fn part_a_example_1() {
        let target_area = get_test_data();
        assert_eq!(try_trajectories(&target_area).0, 45);
    }

    #[test]
    fn part_b_example_1() {
        let target_area = get_test_data();
        assert_eq!(try_trajectories(&target_area).1, 112);
    }
}
