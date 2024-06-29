package org.example;

import java.util.LinkedList;
import java.util.Queue;

public class SharedTasks {
    private volatile Queue<Integer> numsToCheck;


    //private AtomicBoolean isEmpty;

    public SharedTasks(){
        numsToCheck = new LinkedList<>();
        //isEmpty = new AtomicBoolean(true);
    }
    public SharedTasks(Boolean xd){
        numsToCheck = new LinkedList<>();
        for(int i = 2 ; i <= 100000; i++) numsToCheck.add(i);
       // isEmpty = new AtomicBoolean(false);
    }
    public synchronized void addTask(Integer task){
        numsToCheck.add(task);
        notifyAll();

    }
    public synchronized void TrunOffSolvers(){
        ConcurrentTaskSolver.turnOffTasks();
        notifyAll();
    }



    public synchronized Integer getTask(){
       // if(this.numsToCheck.size() == 1) this.isEmpty.set(true);
        //if(this.numsToCheck.isEmpty()) return null;
       // return this.numsToCheck.remove();
        while(this.numsToCheck.isEmpty() && ConcurrentTaskSolver.getExit()){

            try{
                //System.out.println("thread waits");
                wait();
                //System.out.println("threads wake up");

            }catch (InterruptedException e){
                Thread.currentThread().interrupt();
            }
        }
        //notifyAll();
        return this.numsToCheck.poll();

    }

    public synchronized Boolean empty(){
        return this.numsToCheck.isEmpty();
    }

}
