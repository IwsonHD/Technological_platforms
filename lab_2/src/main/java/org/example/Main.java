package org.example;

import java.util.InputMismatchException;
import java.util.Map;
import java.util.Scanner;

public class Main {
    public static void main(String[] args){
        SharedTasks sharedTasks = new SharedTasks();
        SolvedTasks solvedTasks = new SolvedTasks();
        Thread[] concurrentTaskSolvers;
        int amountOfSolvers;
        try{
            amountOfSolvers = Integer.parseInt("4");
            concurrentTaskSolvers = new Thread[amountOfSolvers];

        }catch(NumberFormatException e){
            throw new NumberFormatException("This program only takes integers as arguments");
        }

        for(int i = 0; i < amountOfSolvers; i++){
            concurrentTaskSolvers[i] = new Thread(new ConcurrentTaskSolver(sharedTasks, solvedTasks));
            concurrentTaskSolvers[i].start();
        }

        Scanner scanner = new Scanner(System.in);
        String command;
        int newTask;
        do{
             command = scanner.nextLine();
             switch(command){
                 case "--quit":
                 case "-q":
                     sharedTasks.TrunOffSolvers();
                     break;
                 case "--print_results":
                 case "-p":
                     solvedTasks.printSolvedTasks();
                     break;
                 default:
                     try{
                         newTask = Integer.parseInt(command);
                         if(newTask >= 0) sharedTasks.addTask(newTask);
                     }catch(NumberFormatException e) {
                         System.out.println("Type --quit or -q to exit or input" +
                                 " a valid positive integer in order to add new task");
                     }
                     break;
             }

        }while(ConcurrentTaskSolver.getExit());



        for(int i = 0; i < amountOfSolvers; i++){
            try {
                concurrentTaskSolvers[i].join();
            } catch (InterruptedException e) {
                throw new RuntimeException(e);
            }
        }

        solvedTasks.printSolvedTasks();

    }


}