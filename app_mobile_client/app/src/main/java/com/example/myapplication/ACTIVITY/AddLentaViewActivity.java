package com.example.myapplication.ACTIVITY;

import android.app.ProgressDialog;
import android.content.Context;
import android.content.Intent;
import android.graphics.Bitmap;
import android.graphics.BitmapFactory;
import android.net.ConnectivityManager;
import android.net.NetworkInfo;
import android.net.Uri;
import android.os.Bundle;
import android.util.Base64;
import android.util.Log;
import android.view.View;
import android.widget.Button;
import android.widget.CheckBox;
import android.widget.EditText;
import android.widget.ImageView;
import android.widget.Toast;

import androidx.annotation.Nullable;
import androidx.appcompat.app.AppCompatActivity;

import com.example.myapplication.API.HttpClient;
import com.example.myapplication.API.json_p_lenta;
import com.example.myapplication.CONFIG_USER.ConfigUser;
import com.example.myapplication.GL.GL;
import com.example.myapplication.R;
import com.example.myapplication.SETUP.SetUp;
import com.google.gson.Gson;

import org.json.JSONException;
import org.json.JSONObject;

import java.io.ByteArrayOutputStream;
import java.io.IOException;
import java.io.InputStream;
import java.util.concurrent.CompletableFuture;

public class AddLentaViewActivity extends AppCompatActivity {

    private static final int PICK_IMAGE_REQUEST = 1;
    private EditText namePostEditText, descriptionPostEditText;
    private ImageView postImageView;
    private CheckBox useDefaultImageCheckBox;
    private Bitmap selectedImageBitmap;
    private json_p_lenta lenta_json;
    private Boolean EditPost;

    private String createPostWithImage(Bitmap image) {
        // 1. Сжимаем изображение
        Bitmap compressed = compressImage(image, 800, 800);

        // 2. Конвертируем в WebP вместо JPEG для лучшего сжатия
        ByteArrayOutputStream byteArrayOutputStream = new ByteArrayOutputStream();
        compressed.compress(Bitmap.CompressFormat.WEBP, 75, byteArrayOutputStream);
        byte[] byteArray = byteArrayOutputStream.toByteArray();

        // 3. Используем Base64 без переносов
        return Base64.encodeToString(byteArray, Base64.NO_WRAP);
    }

    private boolean isNetworkAvailable() {
        ConnectivityManager cm = (ConnectivityManager) getSystemService(Context.CONNECTIVITY_SERVICE);
        NetworkInfo activeNetwork = cm.getActiveNetworkInfo();
        return activeNetwork != null && activeNetwork.isConnectedOrConnecting();
    }

    private ProgressDialog showProgressDialog() {
        ProgressDialog dialog = new ProgressDialog(this);
        if (!EditPost)
            dialog.setMessage("Отправка поста...");
        else
            dialog.setMessage("Изминение поста...");
        dialog.setCancelable(false);
        dialog.show();
        return dialog;
    }

    private void showError(String message) {
        Toast.makeText(this, message, Toast.LENGTH_LONG).show();
    }

    private void handleResponse(String response) {
        try {
            Toast.makeText(this, response, Toast.LENGTH_SHORT).show();
            finish();
        } catch (Exception e) {
            showError("Неверный формат ответа сервера");
        }
    }

    private Bitmap compressImage(Bitmap src, int maxWidth, int maxHeight) {
        float ratio = Math.min(
                (float) maxWidth / src.getWidth(),
                (float) maxHeight / src.getHeight()
        );
        int width = Math.round(ratio * src.getWidth());
        int height = Math.round(ratio * src.getHeight());

        return Bitmap.createScaledBitmap(src, width, height, true);
    }

    @Override
    protected void onCreate(@Nullable Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_add_lenta_view);
        new SetUp(this);

        EditPost = getIntent().getBooleanExtra("edit_post", false);



        // Инициализация views
        namePostEditText = findViewById(R.id.namePostEditText);
        descriptionPostEditText = findViewById(R.id.descriptionPostEditText);
        postImageView = findViewById(R.id.postImageView);
        useDefaultImageCheckBox = findViewById(R.id.useDefaultImageCheckBox);
        Button selectImageButton = findViewById(R.id.selectImageButton);
        Button createPostButton = findViewById(R.id.createPostButton);

        if (EditPost)
        {
            createPostButton.setText("Изменить Пост");
            String json = getIntent().getStringExtra("json_lenta");
            lenta_json = new Gson().fromJson(json, json_p_lenta.class);
            namePostEditText.setText(lenta_json.name_post);
            descriptionPostEditText.setText(lenta_json.description_post);
            try {
                byte[] decodedBytes = Base64.decode(lenta_json.image_post, Base64.DEFAULT);
                Bitmap bitmap = BitmapFactory.decodeByteArray(decodedBytes, 0, decodedBytes.length);
                postImageView.setImageBitmap(bitmap);

            } catch (Exception e) {
                e.printStackTrace();
            }


        }
        else
            lenta_json = new json_p_lenta();

        // Обработчик для кнопки выбора изображения
        selectImageButton.setOnClickListener(v -> {
            if (useDefaultImageCheckBox.isChecked()) {
                useDefaultImageCheckBox.setChecked(false);
            }
            openImageChooser();
        });

        // Обработчик для чекбокса
        useDefaultImageCheckBox.setOnCheckedChangeListener((buttonView, isChecked) -> {
            if (isChecked) {
                postImageView.setImageResource(android.R.drawable.ic_menu_camera);
                selectedImageBitmap = null;
            }
        });

        // Обработчик для кнопки создания поста
        createPostButton.setOnClickListener(v -> createPost());
    }

    private void openImageChooser() {
        Intent intent = new Intent();
        intent.setType("image/*");
        intent.setAction(Intent.ACTION_GET_CONTENT);
        startActivityForResult(Intent.createChooser(intent, "Выберите изображение"), PICK_IMAGE_REQUEST);
    }

    @Override
    protected void onActivityResult(int requestCode, int resultCode, @Nullable Intent data) {
        super.onActivityResult(requestCode, resultCode, data);

        if (requestCode == PICK_IMAGE_REQUEST && resultCode == RESULT_OK && data != null && data.getData() != null) {
            Uri imageUri = data.getData();
            try {
                InputStream inputStream = getContentResolver().openInputStream(imageUri);
                selectedImageBitmap = BitmapFactory.decodeStream(inputStream);
                postImageView.setImageBitmap(selectedImageBitmap);
            } catch (IOException e) {
                e.printStackTrace();
                Toast.makeText(this, "Ошибка загрузки изображения", Toast.LENGTH_SHORT).show();
            }
        }
    }

    private void createPost() {
        // Проверка соединения
        if (!isNetworkAvailable()) {
            showError("Нет интернет-соединения");
            return;
        }

        // Валидация данных
        String namePost = namePostEditText.getText().toString().trim();
        String descriptionPost = descriptionPostEditText.getText().toString().trim();

        if (namePost.isEmpty() || descriptionPost.isEmpty()) {
            showError("Заполните все поля");
            return;
        }
            preparePostData(namePost, descriptionPost);
        // Подготовка данных
        json_p_lenta postData = lenta_json;
        if (postData == null) return;

        // Отправка с индикатором прогресса
        ProgressDialog progressDialog = showProgressDialog();

        new Thread(() -> {

            try {
                HttpClient httpClient = new HttpClient();
                String response = "";
                if (!EditPost)
                        response = httpClient.POST(GL.url_api_server + "lenta", new Gson().toJson(postData));
                else
                        response = httpClient.PUT(GL.url_api_server + "lenta", new Gson().toJson(postData));
                String finalResponse = response;
                runOnUiThread(() -> {
                    progressDialog.dismiss();
                    handleResponse(finalResponse);
                });
            } catch (IOException e) {
                runOnUiThread(() -> {
                    progressDialog.dismiss();
                    showError("Ошибка сети: " + e.getMessage());
                    Log.e("Network", "Error sending post", e);
                });
            }
        }).start();
    }

    private void preparePostData(String name, String desc) {
        lenta_json.name_post = name;
        lenta_json.description_post = desc;
        lenta_json.id_user = new ConfigUser(this).get_user().id;

        if (useDefaultImageCheckBox.isChecked()) {
            lenta_json.image_post = "1";
        } else if (selectedImageBitmap != null) {
            try {
                lenta_json.image_post = createPostWithImage(selectedImageBitmap);
            } catch (Exception e) {
                showError("Ошибка обработки изображения");

            }
        } else {
            showError("Выберите изображение");

        }

    }


}

