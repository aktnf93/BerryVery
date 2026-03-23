package com.example.myapplication.network;

import java.util.List;

// Retrofit
// implementation 'com.squareup.retrofit2:retrofit:2.9.0'
// implementation 'com.squareup.retrofit2:converter-gson:2.9.0'
public class RetrofitClient {
    private static final String BASE_URL = "http://10.0.2.2:3000/";
    // 에뮬레이터에서 localhost = 10.0.2.2

    private static Retrofit retrofit;

    public static Retrofit getClient() {
        if (retrofit == null) {
            retrofit = new Retrofit.Builder()
                    .baseUrl(BASE_URL)
                    .addConverterFactory(GsonConverterFactory.create())
                    .build();
        }
        return retrofit;
    }

    public void test() {

        ApiService api = RetrofitClient.getClient().create(ApiService.class);

// GET
        api.getUsers().enqueue(new Callback<List<User>>() {
            @Override
            public void onResponse(Call<List<User>> call, Response<List<User>> response) {
                if (response.isSuccessful()) {
                    List<User> users = response.body();
                }

                if (!response.isSuccessful()) {
                    // 400, 500 처리
                }
            }

            @Override
            public void onFailure(Call<List<User>> call, Throwable t) {
                t.printStackTrace();
            }
        });
    }
}
