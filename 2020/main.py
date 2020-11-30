import runner, sys, os

if __name__ == "__main__":
    if len(sys.argv) > 1:
        runner.run(day=int(sys.argv[1]))
    else:
        numDays = len([x for x in os.listdir(".") if os.path.isdir(x) and x.replace("0","").isnumeric()])
        day = runner.get_day(max_day=numDays)
        runner.run(day=day)
