package com.example.myapplication.ACTIVITY;

import android.os.Bundle;

import androidx.annotation.Nullable;
import androidx.appcompat.app.AppCompatActivity;

import com.example.myapplication.LIST_LENTA.p_lenta_item;
import com.example.myapplication.R;
import com.example.myapplication.SETUP.SetUp;
import com.google.android.material.textview.MaterialTextView;
import com.google.gson.Gson;

public class LentaViewActivity extends AppCompatActivity {

    p_lenta_item lenta;
    @Override
    protected void onCreate(@Nullable Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_lenta_view);
        new SetUp(this);
        String json = getIntent().getStringExtra("lenta_json");
        lenta = new Gson().fromJson(json, p_lenta_item.class);
        MaterialTextView body = findViewById(R.id.body_text);
        MaterialTextView header = findViewById(R.id.header_text);

        header.setText(lenta.getName_post());
        body.setText(lenta.getDescription_post());

    }
}
