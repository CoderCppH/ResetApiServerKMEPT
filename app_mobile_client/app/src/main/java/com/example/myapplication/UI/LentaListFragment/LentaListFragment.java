package com.example.myapplication.UI.LentaListFragment;

import android.content.Intent;
import android.os.Bundle;

import androidx.fragment.app.Fragment;
import androidx.recyclerview.widget.LinearLayoutManager;
import androidx.recyclerview.widget.RecyclerView;

import android.util.Log;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.Button;
import android.widget.ImageButton;

import com.example.myapplication.ACTIVITY.AddLentaViewActivity;
import com.example.myapplication.ACTIVITY.FriendAddActivity;
import com.example.myapplication.ACTIVITY.LentaViewActivity;
import com.example.myapplication.ACTIVITY.MessangerActivity;
import com.example.myapplication.API.HttpClient;
import com.example.myapplication.API.json_p_lenta;
import com.example.myapplication.API.json_p_user;
import com.example.myapplication.GL.GL;
import com.example.myapplication.LIST_LENTA.LentaAdapter;
import com.example.myapplication.LIST_LENTA.p_lenta_item;
import com.example.myapplication.LIST_USER.UserAdapter;
import com.example.myapplication.LIST_USER.p_user_item;
import com.example.myapplication.R;
import com.google.gson.Gson;
import com.google.gson.reflect.TypeToken;

import java.lang.reflect.Type;
import java.util.ArrayList;
import java.util.List;
import java.util.concurrent.CompletableFuture;

public class LentaListFragment extends Fragment {

    private RecyclerView recyclerView;
    private LentaAdapter adapter;
    private List<p_lenta_item> list;

    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container,
                             Bundle savedInstanceState) {
        View view = inflater.inflate(R.layout.fragment_lenta_list, container, false);
        recyclerView = view.findViewById(R.id.recycler_view);
        recyclerView.setLayoutManager(new LinearLayoutManager(getContext()));

        list = new ArrayList<>();
        adapter = new LentaAdapter(inflater, list);

        adapter.setOnItemClickListener(position -> {
            p_lenta_item clickedItem = list.get(position);
            clickedItem.SetNullImage();
            var intent_view_lenta = new Intent(view.getContext(), LentaViewActivity.class);
            intent_view_lenta.putExtra("lenta_json", new Gson().toJson(clickedItem));
            startActivity(intent_view_lenta);
        });

        recyclerView.setAdapter(adapter);

        Init(view);
        fetchData();

        return view;
    }

    private void fetchData() {
        CompletableFuture.runAsync(() -> {
            try {
                HttpClient api = new HttpClient();
                String response = api.GET(GL.url_api_server + "lenta");
                //Log.d("API.GET.LENTA", response);

                Gson gson = new Gson();
                Type lentaListType = new TypeToken<List<json_p_lenta>>() {}.getType();
                List<json_p_lenta> lentaList = gson.fromJson(response, lentaListType);

                for (json_p_lenta lenta : lentaList) {
                    p_lenta_item p_lnt = new p_lenta_item
                            (
                                    lenta.name_post,
                                    lenta.description_post,
                                    lenta.image_post,
                                    lenta.id_user
                            );

                    list.add(p_lnt);
                }

                if(getActivity() == null) return;

                getActivity().runOnUiThread(() -> adapter.notifyDataSetChanged());
            } catch (Exception e) {
                Log.e("API.GET.USERS", "Error fetching data", e);
            }
        });
    }
    private void Init(View view) {
        ImageButton btn = view.findViewById(R.id.btn_add_post);
        btn.setOnClickListener(v -> {
           var intent = new Intent(view.getContext(), AddLentaViewActivity.class);
           startActivity(intent);
        });
    }
   /* private List<p_lenta_item> getData() {
        ArrayList<p_lenta_item> list = new ArrayList<p_lenta_item>();
        p_lenta_item item = new p_lenta_item();
        list.add(item);
        return list;
    }*/
}