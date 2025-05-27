package com.example.myapplication.ACTIVITY;

import android.content.Intent;
import android.graphics.Bitmap;
import android.graphics.BitmapFactory;
import android.os.Bundle;
import android.util.Base64;
import android.util.Log;
import android.view.View;
import android.widget.ImageButton;
import android.widget.ImageView;
import android.widget.Toast;

import androidx.annotation.Nullable;
import androidx.appcompat.app.AppCompatActivity;

import com.example.myapplication.API.HttpClient;
import com.example.myapplication.CONFIG_USER.ConfigUser;
import com.example.myapplication.GL.GL;
import com.example.myapplication.LIST_LENTA.p_lenta_item;
import com.example.myapplication.R;
import com.example.myapplication.SETUP.SetUp;
import com.google.android.material.textview.MaterialTextView;
import com.google.gson.Gson;

import java.io.IOException;
import java.util.function.LongFunction;

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
        ImageView image = findViewById(R.id.image_lenta);

        ImageButton btn_delete = findViewById(R.id.id_delete_btn_lv);
        ImageButton btn_edit = findViewById(R.id.id_edit_btn_lv);

        if (new ConfigUser(this).get_user().id != lenta.getId_user())
        {
            btn_delete.setVisibility(View.GONE);
            btn_edit.setVisibility(View.GONE);
        }

        try {
            byte[] decodedBytes = Base64.decode(lenta.getImage_post(), Base64.DEFAULT);
            Bitmap bitmap = BitmapFactory.decodeByteArray(decodedBytes, 0, decodedBytes.length);
            image.setImageBitmap(bitmap);

        } catch (Exception e) {
            e.printStackTrace();
        }
        header.setText(lenta.getName_post());
        body.setText(lenta.getDescription_post());

    }
    public void DeleteImage(View view)
    {
        new Thread(() -> {
            try {
                HttpClient httpClient = new HttpClient();
                String response = httpClient.DELETE(GL.url_api_server + "lenta/" + lenta.getId() + "/");
                Log.d("logggingndsdf+ds", response);
                if (response != null && response.trim().equals("{\"message\":\"success deleted lenta\"}")) {
                    // Используем саму активити (this)
                    runOnUiThread(() -> {
                        LentaViewActivity.this.finish(); // Замените YourActivityName на реальное имя вашей активити
                    });
                }
            } catch (IOException e) {
                runOnUiThread(() -> {
                    Toast.makeText(LentaViewActivity.this, "Ошибка удаления", Toast.LENGTH_SHORT).show();
                });
                e.printStackTrace();
            }
        }).start();
    }
    public void EditImage(View view)
    {
        var intent = new Intent(view.getContext(), AddLentaViewActivity.class);
        intent.putExtra("edit_post", true);
        intent.putExtra("json_lenta", new Gson().toJson(lenta));
        startActivity(intent);
    }
    public void BackImage(View view)
    {
        finish();
    }
}
