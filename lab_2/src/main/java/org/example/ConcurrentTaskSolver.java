package org.example;

import java.util.NoSuchElementException;
import java.util.concurrent.atomic.AtomicBoolean;

import static java.lang.Math.sqrt;

public class ConcurrentTaskSolver implements Runnable{

    private SharedTasks tasks;


    private SolvedTasks solvedTasks;

    private static volatile AtomicBoolean exit = new AtomicBoolean(true);

    private static int amountOfSolvers = 1;



    private final int solverId;

    public ConcurrentTaskSolver(SharedTasks tasks, SolvedTasks solvedTasks){
        this.solvedTasks = solvedTasks;
        this.tasks = tasks;
        solverId = amountOfSolvers;
        amountOfSolvers++;
    }

    public static boolean getExit(){
        return exit.get();
    }

    public void run(){
        System.out.println("Solver number-" + solverId + " starts solving.");
        while(true) {
            boolean solution = true;
//            try {
//                Thread.sleep(1000);
//            } catch (InterruptedException e) {
//                throw new RuntimeException(e);
//            }

            Integer currTask = tasks.getTask();

            if(currTask == null) continue;

            if(currTask != 0 && currTask != 1) {
                for (int i = 2; i < sqrt((double) currTask); i++) {
                    if (currTask % i == 0) {
                        solution = false;
                        break;
                    }
                }
            }
            //System.out.println("Solver number: " + solverId + " solved " +
            //        currTask + " with a result of " + solution);
            solvedTasks.addSolvedTask(currTask, solution);
        }

    }

    public static void turnOffTasks(){
        exit.set(false);
    }

}



/*
while(tasks.empty()) {
        boolean solution = true;
        Integer currTask = tasks.getTask();

        for (int i = 2; i < sqrt((double) currTask); i++) {
        if (currTask % i == 0) {
        solution = false;
        break;
        }
        try {
        Thread.sleep(0);
        } catch (InterruptedException e) {
        throw new RuntimeException(e);
        }
        }

        solvedTasks.addSolvedTask(currTask, solution);
        }

 */