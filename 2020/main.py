import runner, sys, os,argparse

if __name__ == "__main__":
    argparser = argparse.ArgumentParser()
    argparser.add_argument("-d", "--day", type=int, default=None, help="What day to run")
    argparser.add_argument("-t", "--test", help="Run the tests for the specified day", action="store_true")
    args = argparser.parse_args()

    if args.day == None:
        numDays = len([x for x in os.listdir(".") if os.path.isdir(x) and x.replace("0","").isnumeric()])
        day = runner.get_day(max_day=numDays)
    else:
        day = args.day

    if args.test:
        runner.run_tests(day=day)
    else:
        runner.run(day=day)
