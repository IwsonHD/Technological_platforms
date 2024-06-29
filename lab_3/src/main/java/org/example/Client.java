package org.example;
import java.io.IOException;
import java.io.ObjectInputStream;
import java.io.ObjectOutputStream;
import java.net.Socket;
import java.util.Scanner;

public class Client {
    private Socket socket;
    private ObjectOutputStream outputStream;
    private ObjectInputStream inputStream;
    private String clientUsername;
    private Integer numberOfMessages;
    private Message[] messsagesToSend;

    public Client(Socket socket, String clientUsername, int numberOfMessages){
        try{
            this.socket = socket;
            this.clientUsername = clientUsername;
            this.outputStream = new ObjectOutputStream(socket.getOutputStream());
            this.inputStream = new ObjectInputStream(socket.getInputStream());
            this.numberOfMessages = numberOfMessages;
            messsagesToSend = new Message[numberOfMessages];
            Scanner scanner = new Scanner(System.in);
            for(int i = 0; i < numberOfMessages; i++){
                messsagesToSend[i] = new Message(scanner.nextLine(),i);
            }
        }catch(IOException e){
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

    private void sendMessages(){
        try{
            outputStream.writeObject(this.clientUsername);
            if("Ready" != (String) inputStream.readObject()) closeConnection();
            outputStream.writeInt(numberOfMessages);
            for(int i = 0; i < numberOfMessages; i++){
                outputStream.writeObject(messsagesToSend[i]);
            }
            closeConnection();
        }catch(IOException | ClassNotFoundException e){
            closeConnection();
            e.printStackTrace();
        }
    }
    public static void main(String[] args) throws IOException {

        Client client = new Client(new Socket("localhost", 1234),args[0], Integer.parseInt(args[1]));
        client.sendMessages();
    }

}
