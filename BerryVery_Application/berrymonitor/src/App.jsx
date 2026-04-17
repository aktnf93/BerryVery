import { useState } from 'react'


function TbPort() {
    const res = await axios.get("https://api.example.com/users");
    const data = res.data;

    const res = await axios.post("https://api.example.com/posts", {
        title: "안녕",
        content: "반가워"
    });

};


function App() {
    const [count, setCount] = useState(0)

    return (
        <>
        </>
    );
}

export default App
