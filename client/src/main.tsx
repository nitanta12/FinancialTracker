import './index.css'; 
import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import App from './App';
// Must point to Tailwind entry file
createRoot(document.getElementById('root')!).render(
  <StrictMode>
    <App />
  </StrictMode>,
)
