package com.example.myapplication.LIST_LENTA;

import android.media.Image;

public class p_lenta_item {
    private int id;
    private String name_post;
    private String description_post;
    private String image_post;
    private int id_user;
    private String email;

    public p_lenta_item(int id, String name_post, String description_post, String image_post, int id_user, String email)
    {
        this.email = email;
        this.id = id;
        this.name_post = name_post;
        this.description_post = description_post;
        this.image_post = image_post;
        this.id_user = id_user;
    }

    public String getEmail() {
        return email;
    }

    public void SetNullImage()
    {
        this.image_post = "1";
    }

    public int getId() {
        return id;
    }

    public String getName_post() {
        return name_post;
    }
    public String getDescription_post() {
        return description_post;
    }
    public String getImage_post() {
        return image_post;
    }
    public int getId_user() {
        return id_user;
    }

}
