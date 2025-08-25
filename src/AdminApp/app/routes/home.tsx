import { Sidebar } from "lucide-react";
import ThemeToggle from "~/components/theme-toggle";
import Button from "~/components/button";

const AUTH_API_URL = import.meta.env.VITE_AUTH_API_URL + 'auth' || 'test';

export default function Home() {
  return (
    <>
      <div>This is home page</div>
      <div><Sidebar /></div>
      <div><ThemeToggle /></div>
      <Button leadingIcon={<span>💾</span>} onClick={() => {console.log(AUTH_API_URL)}}>Save</Button>
    </>
  );
}
