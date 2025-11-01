import './index.css'; 
import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import App from './App';
import { Provider } from 'react-redux';
import { store } from './redux/store';
// Must point to Tailwind entry file
createRoot(document.getElementById('root')!).render(
  <StrictMode>
    <Provider store = {store}>

    <App />
    </Provider>
  </StrictMode>,
)
