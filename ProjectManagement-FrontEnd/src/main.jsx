// import { createRoot } from 'react-dom/client'
// import './index.css'
// import App from './App.jsx'
// import { BrowserRouter } from 'react-router-dom'
// import { store } from './app/store.js'
// import { Provider } from 'react-redux'

// createRoot(document.getElementById('root')).render(
//     <BrowserRouter>
//             <Provider store={store}>
//                 <App />
//             </Provider>
//     </BrowserRouter>,
// )

import ReactDOM from "react-dom/client";
import './index.css';
import App from "./App";
import { store, persistor } from "./features/store";
import { Provider } from "react-redux";
import { PersistGate } from "redux-persist/integration/react";
import { BrowserRouter } from "react-router-dom";

ReactDOM.createRoot(document.getElementById("root")).render(
    <Provider store={store}>
        <PersistGate loading={null} persistor={persistor}>
            <BrowserRouter>
                <App />
            </BrowserRouter>
        </PersistGate>
    </Provider>
);

