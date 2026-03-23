package com.example.myapplication.network;

import java.io.BufferedReader;
import java.io.InputStreamReader;
import java.io.OutputStream;
import java.net.HttpURLConnection;
import java.net.URL;

public class HttpUrlClient {
    public void getUsers() {
        new Thread(() -> {
            try {
                URL url = new URL("http://10.0.2.2:3000/users");
                HttpURLConnection conn = (HttpURLConnection) url.openConnection();

                String http_method = "GET";

                if (http_method == "GET") {
                    conn.setRequestMethod("GET");
                    conn.setConnectTimeout(5000);
                    conn.setReadTimeout(5000);
                }
                else {
                    conn.setRequestMethod("POST");
                    conn.setRequestProperty("Content-Type", "application/json");
                    conn.setDoOutput(true);

                    String jsonInput = "{\"name\":\"Kim\"}";

                    OutputStream os = conn.getOutputStream();
                    os.write(jsonInput.getBytes("UTF-8"));
                    os.close();
                }


                // _________________________________________
                int responseCode = conn.getResponseCode();
                BufferedReader reader = new BufferedReader(new InputStreamReader(conn.getInputStream()));
                StringBuilder result = new StringBuilder();
                String line;

                while ((line = reader.readLine()) != null) {
                    result.append(line);
                }

                reader.close();

                String data = result.toString();

            } catch (Exception e) {
                e.printStackTrace();
            }
        }).start();
    }
}
