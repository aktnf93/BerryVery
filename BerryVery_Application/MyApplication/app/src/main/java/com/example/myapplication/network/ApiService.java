package com.example.myapplication.network;

import android.telecom.Call;

import java.util.List;

public interface ApiService {
    @GET("users")
    Call<List<User>> getUsers();

    @POST("users")
    Call<User> createUser(@Body User user);

    @PUT("users/{id}")
    Call<User> updateUser(@Path("id") int id, @Body User user);

    @DELETE("users/{id}")
    Call<Void> deleteUser(@Path("id") int id);
}
