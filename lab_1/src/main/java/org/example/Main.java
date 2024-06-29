package org.example;

public class Main {
    public static void main(String[] args) {
        int mode;
        if(args.length == 0) mode = 0;
        else {
            switch (args[0]) {
                case "--no_sort":
                    mode = 0;
                    break;
                case "--sort_norm":
                    mode = 1;
                    break;
                case "--sort_alter":
                    mode = 2;
                    break;
                default:
                    System.out.println("Wrong args");
                    return;
            }
        }
        Mage mag1 = new Mage("iwo", 12, 1.3, mode);
        Mage mag2 = new Mage("adam", 12, 1.3, mode);
        Mage mag3 = new Mage("kacper", 120, 1.3, mode);
        Mage mag4 = new Mage("adasko", 1000000000, 10000.123, mode);
        Mage mag5 = new Mage("zuzia", 13, 1.3, mode);
        Mage mag6 = new Mage("agata", 16, 1.3, mode);
        Mage mag7 = new Mage("madzia", 723, 1.3, mode);
        Mage mag8 = new Mage("henio", 133, 1.3, mode);
        Mage mag9 = new Mage("chinczyk_jebany_smiec", -1000000, -100, mode);
        Mage mag10 = new Mage("kicia", 696969696,6.9 , mode);//<3

        //mag1.addApprentice(mag9);
        //mag1.addApprentice(mag10);
        //mag1.addApprentice(mag5);
       // mag1.addApprentice(mag8);
        //mag10.addApprentice(mag1);
        //mag10.addApprentice(mag9);
        //mag3.addApprentice(mag2);
        //mag4.addApprentice(mag1);
        //mag5.addApprentice(mag9);
        //mag5.addApprentice(mag3);
        //mag1.addApprentice(mag4);
        mag10.addApprentice(mag3);
        mag10.addApprentice(mag5);
        mag5.addApprentice(mag3);


        mag10.showApprentices();

        mag10.showStatistics();





    }
}