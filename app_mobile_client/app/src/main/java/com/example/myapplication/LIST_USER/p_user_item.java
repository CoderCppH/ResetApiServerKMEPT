package com.example.myapplication.LIST_USER;

public class p_user_item {
    int image;
    String fullname, email;
    public p_user_item(int image, String fullname, String email) {
        this.image = image;
        this.fullname = fullname;
        this.email = email;
    }
    public int getImage() {
        return this.image;
    }
    public String getFullName() {
        return this.fullname;
    }
    public String getEmail() {
        return this.email;
    }
}
