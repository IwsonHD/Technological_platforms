package org.example;

import java.io.*;
import java.net.Socket;

public class ClientHandler implements Runnable{
    private Server mainServer;
    private Socket socket;
    private ObjectInputStream inputStream;
    private ObjectOutputStream outputStream;
    private String clientName;
    private Integer numberOfMessages;
    public ClientHandler(Socket socket, Server server){
        try {
            this.mainServer = server;
            this.socket = socket;
            this.outputStream = new ObjectOutputStream(socket.getOutputStream());
            this.inputStream = new ObjectInputStream(socket.getInputStream());
            this.clientName = (String) inputStream.readObject();
            this.outputStream.writeObject("Ready");
            this.numberOfMessages = inputStream.readInt();
        }
        catch(IOException | ClassNotFoundException e){
            closeConnection();
            e.printStackTrace();
        }
    }

    private void closeConnection(){
        try{
            if(this.inputStream != null) this.inputStream.close();
            if(this.outputStream != null) this.outputStream.close();
            if(this.socket != null) this.socket.close();
        }catch(IOException e){
            e.printStackTrace();
        }
    }

    @Override
    public void run(){
        try {
            Message[] messages = new Message[numberOfMessages];
            for(int i = 0; i < numberOfMessages; i++){
                messages[i] =(Message) inputStream.readObject();
            }

        }catch(IOException | ClassNotFoundException e){
            closeConnection();
            e.printStackTrace();
        }
    }
}
