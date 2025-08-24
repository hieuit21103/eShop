import { Sidebar } from "lucide-react";
import ThemeToggle from "~/components/theme-toggle";
import Button from "~/components/button";

export default function Home() {
  return (
    <>
      <div>This is home page</div>
      <div><Sidebar /></div>
      <div><ThemeToggle /></div>
      <Button leadingIcon={<span>💾</span>}>Save</Button>
    </>
  );
}
