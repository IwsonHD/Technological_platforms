package org.example;

import java.io.Serializable;

public class Message implements Serializable {
    private int number;
    private String content;

    public Message(String content, int number){
        this.number = number;
        this.content = content;
    }

    public String getContent() {
        return content;
    }

    public void setContent(String content) {
        this.content = content;
    }

    public void setNumber(int number) {
        this.number = number;
    }

    public int getNumber() {
        return number;
    }
}
