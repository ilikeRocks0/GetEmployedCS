"use client";
import Link from "next/link";

export default function NavigationBar() {
    return (
        <div className="flex h-20 bg-blue-200">
            <div className="my-auto flex w-full">
            <Link href="/" className="inline px-5 pr-0 font-bold">GetEmployed.CS</Link>

            <div className="grid grid-cols-4 gap-x-10 justify-self-center">
                <Link href="/search-jobs" className="inline text-end">Job List</Link>
                <p className="inline">Writing Help</p>
                <p className="inline">Manage Jobs</p>
                <p className="inline">Add Jobs</p>
            </div>

            <div className="absolute right-3 top-2 border w-15 h-15 rounded-4xl bg-white">
                <p className="my-4 text-center">Profile</p>    
            </div>    
            </div>
        </div>
    )    
}