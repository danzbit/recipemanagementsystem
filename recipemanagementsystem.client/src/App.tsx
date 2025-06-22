import AppRouter from './routes/AppRouter';
import { ToastContainer } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';

function App() {
  return (
    <>
      <div className="App">
        <AppRouter />
      </div>
      <ToastContainer position="top-right" autoClose={3000} />
    </>
  );
}

export default App;
