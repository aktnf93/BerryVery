package com.example.berryandroid.ui.main;
import com.example.berryandroid.R;

import android.os.Bundle;
import android.view.View;
import android.widget.Button;
import android.widget.FrameLayout;

import androidx.appcompat.app.AppCompatActivity;
import androidx.fragment.app.Fragment;

public class MainActivity extends AppCompatActivity {
    private FrameLayout container;
    Button myButton;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);

        // 2. ID 연결 (R.id.이름 형식으로 접근)
        myButton = findViewById(R.id.my_button);

        // 3. 사용 예시 (클릭 리스너 설정)
        myButton.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                myButton.setText("버튼이 눌렸어요!");
            }
        });

        // container = findViewById(R.id.fragment_container);

        // 초기 화면
        // loadFragment(new Menu1Fragment());
    }
/*
    private void loadFragment(Fragment fragment) {
        getSupportFragmentManager()
                .beginTransaction()
                .replace(R.id.fragment_container, fragment)
                .commit();
    }
*/
    // 메뉴 클릭 시
    public void onMenu1Click() {
        // loadFragment(new Menu1Fragment());
    }

    public void onMenu2Click() {
        // loadFragment(new Menu2Fragment());
    }
}
