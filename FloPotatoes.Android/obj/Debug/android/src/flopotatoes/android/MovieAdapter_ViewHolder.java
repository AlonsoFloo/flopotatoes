package flopotatoes.android;


public class MovieAdapter_ViewHolder
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer
{
	static final String __md_methods;
	static {
		__md_methods = 
			"";
		mono.android.Runtime.register ("FloPotatoes.Android.MovieAdapter/ViewHolder, FloPotatoes.Android, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", MovieAdapter_ViewHolder.class, __md_methods);
	}


	public MovieAdapter_ViewHolder () throws java.lang.Throwable
	{
		super ();
		if (getClass () == MovieAdapter_ViewHolder.class)
			mono.android.TypeManager.Activate ("FloPotatoes.Android.MovieAdapter/ViewHolder, FloPotatoes.Android, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}

	public MovieAdapter_ViewHolder (android.widget.TextView p0, android.widget.ImageView p1, android.widget.ImageView p2, android.widget.TextView p3, android.widget.TextView p4, android.widget.TextView p5, android.widget.TextView p6, android.widget.TextView p7, android.widget.TextView p8) throws java.lang.Throwable
	{
		super ();
		if (getClass () == MovieAdapter_ViewHolder.class)
			mono.android.TypeManager.Activate ("FloPotatoes.Android.MovieAdapter/ViewHolder, FloPotatoes.Android, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "Android.Widget.TextView, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=84e04ff9cfb79065:Android.Widget.ImageView, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=84e04ff9cfb79065:Android.Widget.ImageView, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=84e04ff9cfb79065:Android.Widget.TextView, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=84e04ff9cfb79065:Android.Widget.TextView, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=84e04ff9cfb79065:Android.Widget.TextView, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=84e04ff9cfb79065:Android.Widget.TextView, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=84e04ff9cfb79065:Android.Widget.TextView, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=84e04ff9cfb79065:Android.Widget.TextView, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=84e04ff9cfb79065", this, new java.lang.Object[] { p0, p1, p2, p3, p4, p5, p6, p7, p8 });
	}

	java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}
