package org.example;

import java.io.IOException;
import java.net.ServerSocket;
import java.net.Socket;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.HashSet;
import java.util.TreeSet;
import java.util.concurrent.ConcurrentHashMap;
import java.util.concurrent.atomic.AtomicBoolean;

public class Server {
    private ServerSocket serverSocket;
    private ArrayList<Thread> currentClients;
    private volatile ConcurrentHashMap<String, Message[]> messagesRecived;

    private volatile AtomicBoolean serverUp;

    public Server(ServerSocket serverSocket){
        this.serverSocket = serverSocket;
        this.messagesRecived = new ConcurrentHashMap<>();
        this.currentClients = new ArrayList<>();

        this.serverUp = new AtomicBoolean(true);
    }


    public void startServer(){
        try{
            while(!serverSocket.isClosed()){
                Socket socket = serverSocket.accept();
                System.out.println("New client has connected");
                ClientHandler clientHandler = new ClientHandler(socket,this);
                Thread newClientThread = new Thread(clientHandler);
                this.currentClients.add(newClientThread);
                newClientThread.start();
            }
        }catch(IOException e){
            e.printStackTrace();
        }
    }

    public Boolean getServerStatus(){
        return this.serverUp.get();
    }
    private void closeServer() throws InterruptedException{
        try{
            if(serverSocket != null){
                serverSocket.close();
                this.serverUp.set(false);
                for(Thread clientsHandlers : this.currentClients){
                    clientsHandlers.interrupt();
                    clientsHandlers.join();
                }
            }
        }catch(IOException e){
            e.printStackTrace();
        }
    }

    public synchronized void appendNewMessage(String clientName, Message messageReceived){

    }



    public static void main(String[] args) throws IOException{
        ServerSocket serverSocket = new ServerSocket(1234);
        Server server = new Server(serverSocket);
        server.startServer();
    }


}
