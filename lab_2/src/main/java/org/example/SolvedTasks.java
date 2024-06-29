package org.example;

import java.util.HashMap;
import java.util.Map;
import java.util.Set;

public class SolvedTasks {
    private HashMap<Integer, Boolean> solvedTasks;

    SolvedTasks(){
        this.solvedTasks = new HashMap<Integer, Boolean>();
    }

    public synchronized void addSolvedTask(Integer task, Boolean solution){
        this.solvedTasks.put(task, solution);
    }

    public synchronized Boolean getSolution(Integer task){
        if(!solvedTasks.containsKey(task)){
            throw new IllegalArgumentException("This task has not been solved yet");
        }
        return this.solvedTasks.get(task);
    }

    public Set<Map.Entry<Integer, Boolean>> entrySet(){
        return solvedTasks.entrySet();
    }

    public void printSolvedTasks(){
        for(Map.Entry<Integer,Boolean> solvedTask : this.solvedTasks.entrySet()){
            System.out.println("Task " + solvedTask.getKey() + " with a result of " + solvedTask.getValue());
        }
    }

}
