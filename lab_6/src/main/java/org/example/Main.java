package org.example;

import org.apache.commons.lang3.tuple.Pair;

import javax.imageio.ImageIO;
import java.awt.*;
import java.awt.image.BufferedImage;
import java.io.BufferedReader;
import java.io.IOException;
import java.nio.file.Files;
import java.nio.file.Path;
import java.util.List;
import java.util.concurrent.ExecutionException;
import java.util.concurrent.ForkJoinPool;
import java.util.stream.Collectors;
import java.util.stream.Stream;

public class Main {
    public static void main(String[] args) throws IllegalArgumentException {

        if(args.length != 2){
            throw new IllegalArgumentException("Input and output directory must be given");
        }

        Path source = Path.of(args[0]);
        Path outputPath = Path.of(args[1]);
        try(Stream<Path> stream = Files.list(source)){
            List <Path> files = stream.collect(Collectors.toList());

            long startTime = System.currentTimeMillis();
            ForkJoinPool threadPool = new ForkJoinPool(5);
            threadPool.submit(
                    () -> files.parallelStream()
                            .map(file -> {
                                try{
                                    return Pair.of(file.getFileName().toString(), ImageIO.read(file.toFile()));
                                }catch(IOException e){
                                    e.printStackTrace();
                                    return null;
                                }
                            })
                            .map(pair -> {
                                if(pair == null) return null;
                                BufferedImage newImage = new BufferedImage(
                                        pair.getRight().getWidth(),
                                        pair.getRight().getHeight(),
                                        pair.getRight().getType()
                                );
                                for( int i = 0; i < pair.getRight().getWidth(); i++){
                                    for(int j = 0; j < pair.getRight().getHeight(); j++){
                                        int rgb = pair.getRight().getRGB(i,j);
                                        Color color = new Color(rgb);
                                        int red = color.getRed();
                                        int blue = color.getBlue();
                                        int green = color.getGreen();
                                        Color outColor = new Color(red,blue, green);
                                        int outRgb = outColor.getRGB();
                                        newImage.setRGB(i,j,outRgb);
                                    }
                                }
                                return Pair.of(pair.getLeft(), newImage);
                            })
                            .forEach(pair ->{
                                if(pair == null) return;

                                Path outPath = outputPath.resolve(pair.getLeft());
                                try{
                                    ImageIO.write(pair.getRight(),"png",outPath.toFile());
                                }catch(IOException e){
                                    e.printStackTrace();
                                }
                            })
            ).get();

            long endTime = System.currentTimeMillis();
            System.out.println("Processing time: " + (endTime - startTime) + " ms");
            threadPool.shutdown();
        }catch(IOException | InterruptedException | ExecutionException e) {
            e.printStackTrace();
        }
    }
}