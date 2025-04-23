package com.example.myapplication.ACTIVITY;

import android.os.Bundle;
import android.util.Log;

import androidx.annotation.Nullable;
import androidx.appcompat.app.AppCompatActivity;
import androidx.recyclerview.widget.LinearLayoutManager;
import androidx.recyclerview.widget.RecyclerView;

import com.example.myapplication.API.ApiClient;
import com.example.myapplication.GL.GL;
import com.example.myapplication.LIST_USER.UserAdapter;
import com.example.myapplication.LIST_USER.p_user_item;
import com.example.myapplication.R;

import java.util.ArrayList;
import java.util.List;

public class FriendAddActivity extends AppCompatActivity {

    private RecyclerView recyclerView;
    private UserAdapter adapter;

    @Override
    protected void onCreate(@Nullable Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_add_friend);
        recyclerView = findViewById(R.id.recycle_view_friends_on_add);
        recyclerView.setLayoutManager(new LinearLayoutManager(this));
        new Thread(()->{

            ApiClient api = new ApiClient();
            var temp = api.GET( GL.url_api_server  + "get_list_user");
            Log.d("API.GET.USERS", temp);

            runOnUiThread(()->{
                adapter = new UserAdapter(this.getLayoutInflater(), getData());
                recyclerView.setAdapter(adapter);
            });
        }).start();

    }
    private List<p_user_item> getData() {
        ArrayList<p_user_item> list = new ArrayList<p_user_item>();




        return list;
    }
}
